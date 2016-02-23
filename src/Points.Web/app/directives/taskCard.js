'use strict';
app.directive('taskCard', function () {
    return {
        scope: {
            columnSize: '@'
        },
        templateUrl: '/app/views/directives/taskCard.html',
        transclude: true,
        controller: "taskCardController"
    };
}).controller('taskCardController', ['$scope', function ($scope) {

    $scope.colWidth = "width:{0}px".format($scope.columnSize);
}]);