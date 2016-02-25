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
    $scope.user = {};
    $scope.hideCats = true;
    $scope.totalClass = 'active';

    var calculateTotalClass = function () {
        if ($scope.user.targetPoints && $scope.totals.points) {
            var pct = $scope.totals.points * 100 / $scope.user.targetPoints;
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
            calculateTotalClass();
        });
        resourceService.get('users', function (data) {
            $scope.user = data[0];
            calculateTotalClass();
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