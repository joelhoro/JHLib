angular.module('webSocketApp', [])
.controller('messageList',[ '$scope', function($scope) {
	$scope.messages = [];
	$scope.trash = [];
	$scope.id = 1;
	$scope.trashVisible = false;

	$scope.addMessage = function() {
	    var message = { id: $scope.id, content: "Message #" + $scope.id++ };
		$scope.messages.push(message);
	}
	$scope.moveToTrash = function(message) {
		if(message == undefined) {
			// move all to trash
			$.merge($scope.trash,$scope.messages);
			$scope.messages = [];
		}
		else {
			// only move that message to trash
			$.merge($scope.trash,message);
			var index = $scope.messages.indexOf(message);
			$scope.messages.splice(index,1)
		}
	}
	$scope.toggleTrashVisibility = function() {
		$scope.trashVisible = !$scope.trashVisible;
	}
	$scope.emptyTrash = function() {
		$scope.trash = [];
	}
	$scope.restoreTrash = function() {
		$scope.messages = $scope.trash.concat($scope.messages);
		$scope.emptyTrash();
	}

	$scope.initialize = function() {
		$scope.addMessage();
		$scope.addMessage();
		$scope.addMessage();
		$scope.addMessage();
		$scope.moveToTrash();
		$scope.addMessage();	
	}
	
} ] )
// message-list template directive
.directive('messageList', function() {
	var tpl = `
	<div class=panel ng-repeat='model in list'>
			<div class='panel-body' id='mess{{model.id}}'>
				<button class='btn btn-danger'ng-click='moveToTrashFn(model)'>
					<icon icon-name='trash'></icon>
				</button>
				{{model.content}}
			</div>
	<\div>
	`
	return {
		restrict: 'E',
		scope: { list : '=', moveToTrashFn : '=' },
		template: tpl
	    	}
	}
)
.directive('icon', function() {
	return {
		restrict: 'E',
		scope : { iconName : '@'},
		template: `<span class='glyphicon glyphicon-{{iconName}}'></span>`
	}
});

