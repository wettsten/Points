'use strict';
app.directive('taskCard', function () {
    return {
        scope: {
            tasksPerRow: '@'
        },
        templateUrl: '/app/views/directives/taskCard.html',
        transclude: true,
        controller: "taskCardController"
    };
}).controller('taskCardController', ['$scope', function ($scope) {

    $scope.colWidth = "col-md-{0}".format(12 / $scope.tasksPerRow);
}]);