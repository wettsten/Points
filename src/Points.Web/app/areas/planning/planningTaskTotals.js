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
    var user = {};
    $scope.hideCats = true;
    $scope.totalClass = 'active';

    $scope.calculateTotalClass = function () {
        if (user.targetPoints && $scope.totals.points) {
            var pct = $scope.totals.points * 100 / user.targetPoints;
            if (pct >= 100) {
                $scope.totalClass = 'success';
            } else if (pct >= 50) {
                $scope.totalClass = 'warning';
            } else if (pct > 0) {
                $scope.totalClass = 'danger';
            } else {
                $scope.totalClass = 'active';
            }
        }
    };

    var loadData = function () {
        resourceService.get('planningtotals', function (data) {
            $scope.totals = data;
            angular.forEach($scope.totals.categories, function (cat) {
                cat.hideTasks = true;
            });
            $scope.calculateTotalClass();
        });
        resourceService.get('users', function (data) {
            user = data[0];
            $scope.calculateTotalClass();
        });
    };

    $scope.toggleCats = function () {
        $scope.hideCats = !$scope.hideCats;
        angular.forEach($scope.totals.categories, function (cat) {
            cat.hideTasks = true;
        });
    };

    $scope.toggleTasks = function (cat) {
        cat.hideTasks = !cat.hideTasks;
    };

    loadData();
}]);