angular.module('pivotTableService')
.service('PivotTable', function(utils,_,$) {
class PivotTable {
            constructor(table,source,pivots,valueFields,classes) {
                  this.table = table;
                  this.source = source;
                  this.nodesDictionary = [{level:0, filter: {}, open: false}];
                  this.valueFields = valueFields;
                  this.pivots = pivots;
                  this.id = 0;
                  var template = _.template(`
                  <tr data-tt-id='<%= newid %>' data-tt-parent-id='<%= parentid %>' >
                        <td>
                              <span class='<%= classes[pivotLevel] %>'><%= key %></span>
                        </td>
                        <%= values %>
                  </tr>`);
                  this.classes = classes;
                  this.rowTemplate = function(obj) {
                        var lastElement = classes[classes.length-1];
                        for(var i=1;i<10;i++) { classes.push(lastElement) }
                        obj.classes = classes;

                        obj.pivots = pivots;
                        return template(obj);
                  }

                  this.initialize();
            }

            initialize() {

                  // create the table
                  this.table.treetable({ expandable: true }).treetable("expandAll");
                  // create the column names based on the fields
                  var firstColumName = this.pivots.join(" &gt;&gt; ");
                  var columns = [ firstColumName ].concat(this.valueFields);
                  this.table
                        .find("thead")
                        .append(columns.map(utils.HTMLWrapper("th")));

                  // create the root node
                  var values = this.valueFields
                        .map(x => "")
                        .map(utils.HTMLWrapper("td"))
                        .join("\n");
                  var root = 
                  { 
                        parentid: -1, 
                        newid : 0, 
                        key: "Sales database", 
                        value: "", 
                        pivotLevel: 0, 
                        values: values,
                  };

                  this.addSingleNode(root,null);

                  // open the root node
                  this.queryNode(0);

            }
            
            addNode(baseNode, obj, currentField, pivotLevel) {
                  var parentid;
                  if(baseNode == null)
                        parentid = 0;
                  else
                        parentid = baseNode.id;
                  this.id++;
                  var id = this.id;
                  this.nodesDictionary[id] = {level: pivotLevel, open: false};
                  this.nodesDictionary[id].filter = _.clone(this.nodesDictionary[parentid].filter)
                  this.nodesDictionary[id].filter[currentField]=obj.Key;

                  var values = this
                        .valueFields
                        .map(utils.ObjectFn(obj.Values))
                        .map(x => x.toFixed(2))
                        .map(utils.HTMLWrapper("td","class='node-level-"+Math.min(pivotLevel,3)+"'"))
                        .join("\n");
                  this.addSingleNode({ parentid: parentid, newid : id, key: obj.Key, 
                        pivotLevel: pivotLevel, values: values }, baseNode );
            }

            addSingleNode(specs, baseNode) {
                  var newNode = this.rowTemplate(specs);
                  this.table.treetable("loadBranch", baseNode, newNode );
                  var thisCopy = this;
                  var newNode = $("[data-tt-id="+specs.newid+"]");
                  newNode.click(x => thisCopy.queryNode(specs.newid));
//                  newNode.on('hover','', function() { var details = thisCopy.nodesDictionary[specs.newid]; debugger; });
            }

            // query node id
            queryNode(id) {
                        var node = this.table.treetable("node",id);
                        var nodeInfo = this.nodesDictionary[id];
                        // if this node has already been opened, just toggle it
                        if (nodeInfo.opened) {
                              node.toggle();
                              return;
                        }
                        nodeInfo.opened = true;
                        var filter = nodeInfo.filter;
                        var pivotLevel = nodeInfo.level;
                        var nextPivot = this.pivots[pivotLevel];

                        var thisCopy = this;
                        this.source.drilldown(filter, nextPivot, this.valueFields, function(newData) {
                              newData.map(row => thisCopy.addNode(node, row, nextPivot, pivotLevel+1))
                              
                        });

                  }
      }

      return PivotTable;
})

      