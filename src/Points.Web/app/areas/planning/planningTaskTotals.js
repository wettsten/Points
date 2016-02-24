'use strict';
app.directive('planningTaskTotals', function () {
    return {
        scope: {
        },
        templateUrl: '/app/views/directives/planningTaskTotals.html',
        replace: true,
        controller: 'planningTaskTotalsController'
    };
}).controller('planningTaskTotalsController', ['$scope', 'resourceService', function ($scope, resourceService) {

    $scope.totals = {};
    $scope.hideCats = true;

    var loadTotals = function () {
        resourceService.get('planningtotals');
    };

    resourceService.subscribe('planningtotals', function (data) {
        $scope.totals = data;
        angular.forEach($scope.totals.categories, function (cat) {
            cat.hideTasks = true;
        });
    });

    $scope.toggleCats = function () {
        $scope.hideCats = !$scope.hideCats;
        angular.forEach($scope.totals.categories, function (cat) {
            cat.hideTasks = true;
        });
    };

    $scope.toggleTasks = function (cat) {
        cat.hideTasks = !cat.hideTasks;
    };

    loadTotals();
}]);