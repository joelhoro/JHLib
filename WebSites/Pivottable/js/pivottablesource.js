angular
.module('pivotTableService',['utilsService'])
.service('PivotTableSourceFromTable', function(utils,_) {

      class PivotTableSourceFromTable {

            constructor(data, dimensions, metrics) {
                  console.log("Creating source from %s records, with dimensions=%o and metrics=%o", data.length, dimensions, metrics);
                  console.log("Fields: %o", _.keys(data[0]));                  
                  this.data = data;
                  this.dimensions = dimensions;
                  this.metrics = metrics;
            }

            // returns a list of rows where each row has key = is one possible value for 'nextPivot', 
            // and all the aggregated valueFields
            drilldown(filter, nextPivot, valueFields, successFn) {
                  console.log("[%s] Drilling down %s, filter=%s => metrics=%o", this.constructor.name, nextPivot, JSON.stringify(filter), valueFields);
                  // get all the filtered data, grouped by nextPivot
                  var groupedData = this.data
                        .where(filter)
                        .groupBy(utils.FieldExtractor(nextPivot));
                  var keys = _.keys(groupedData).sortBy();
                  console.log("Found %s groups: %o", keys.length, keys );
                  // for each group, summarize the results
                  var returnValue = _.map(keys, function(key) {
                        var values = valueFields
                              .toObject(field => groupedData[key].sum(utils.FieldExtractor(field)));
                        return { Key: key, Values: values };
                  });
                  
                  successFn(returnValue);
            }

            // returns the list of possible values for a dimension, optionally, with a filter
            distinctValues(dimension, filter = {}) {
                  var groupBy = this.data
                        .where(filter)
                        .groupBy(utils.FieldExtractor(dimension));
                  return _.map(groupBy, (values,key) => key);
            }
      }

      return PivotTableSourceFromTable;      

} ) // factory('PivotTableSourceFromTable', function(utils) ...
.service('PivotTableSourceAjax', function(utils,_,$http) {

      class PivotTableSourceAjax {

            constructor(url,settings,dimensions, metrics) {
                  this.url = url;
                  this.settings = settings;
                  this.dimensions = dimensions;
                  this.metrics = metrics;
            }

            // returns a list of rows where each row has key = is one possible value for 'nextPivot', 
            // and all the aggregated valueFields
            drilldown(filter, nextPivot, valueFields, successFn) {

                  var results;                  
                  var queryData = { 
                        'queryParams' : { filter: filter, nextPivot: nextPivot, valueFields: valueFields }, 
                        'settings' : this.settings 
                  };

                  console.debug("Querying server for ", JSON.stringify(queryData));
                  $.blockUI({ message: '<h1>Loading data...</h1>' });

                  $http({
                    method: "POST",
                    url: this.url,
                    data: JSON.stringify(queryData),
                  }).then(result => {
                        $.unblockUI();
                        console.debug("Results downloaded from server: %s", result.data.d.length); 
                        successFn(result.data.d);
                  }); 
            }

            // returns the list of possible values for a dimension, optionally, with a filter
            distinctValues(dimension, filter = {}) {

            }
      }

      return PivotTableSourceAjax;      

} ); // factory('PivotTableSourceFromTable', function(utils) ...
