angular.module('utilsService',[])
.service('_', function() { var _ = window._; return _; })
.service('$', function() { var $ = window.$; return $; })
.service('utils', function(_,$) {
	// Generic array functions
	Array.prototype.sum = function(fn = x=>x) { 
	      return this.map(fn).reduce((a,b) => a+b) 
	}

	Array.prototype.where = function(values) { return _.where(this,values); }
	Array.prototype.groupBy = function(fn) { return _.groupBy(this,fn); }
	Array.prototype.sortBy = function(fn) { return _.sortBy(this,fn); }
	Array.prototype.toObject = function(fn) { 
	      var obj = {}; 
	      this.forEach(field => obj[field] = fn(field));
	      return obj;
	}

	String.prototype.contains = function(substr) { return this.indexOf(substr)>-1 };
	
	return { 
		FieldExtractor	: fieldName => obj => obj[fieldName], 
		ObjectFn		: obj => fieldName => obj[fieldName],
		HTMLWrapper		: (tagStyle,attributes="") => x => "<"+tagStyle+" " + attributes + ">"+x+"</"+tagStyle+">"
	};	
})

	