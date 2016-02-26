'use strict';
app.directive('planningTaskTotals', function () {
    return {
        scope: {
        },
        templateUrl: '/app/views/directives/planningTaskTotals.html',
        replace: true,
        controller: 'planningTaskTotalsController'
    };
}).controller('planningTaskTotalsController', ['$scope', 'resourceService', 'filterFactory', function ($scope, resourceService, filterFactory) {

    $scope.totals = {};
    $scope.user = {};
    $scope.hideCats = true;
    $scope.totalClass = 'active';
    $scope.hasFilter = false;
    var toggle = true;

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
                cat.filter = false;
                angular.forEach(cat.tasks, function (task) {
                    task.filter = false;
                });
            });
            calculateTotalClass();
        });
        resourceService.get('users', function (data) {
            $scope.user = data[0];
            calculateTotalClass();
        });
    };

    $scope.toggleCats = function () {
        if (!toggle) {
            toggle = true;
            return;
        }
        $scope.hideCats = !$scope.hideCats;
        angular.forEach($scope.totals.categories, function (cat) {
            cat.hideTasks = true;
        });
    };

    $scope.toggleTasks = function (cat) {
        if (!toggle) {
            toggle = true;
            return;
        }
        cat.hideTasks = !cat.hideTasks;
    };

    $scope.filterTask = function (task) {
        task.filter = !task.filter;
        angular.forEach($scope.totals.categories, function (cat) {
            if (cat.filter) {
                $scope.hasFilter = true;
                return;
            }
            angular.forEach(cat.tasks, function(task) {
                if (task.filter) {
                    $scope.hasFilter = true;
                    return;
                }
            });
        });
    };

    $scope.filterCat = function (cat) {
        toggle = false;
        cat.filter = !cat.filter;
        angular.forEach($scope.totals.categories, function (cat) {
            if (cat.filter) {
                $scope.hasFilter = true;
                return;
            }
        });
    };

    $scope.clearFilter = function () {
        toggle = false;
        if ($scope.hasFilter) {
            $scope.hasFilter = false;
            angular.forEach($scope.totals.categories, function (cat) {
                cat.filter = false;
                angular.forEach(cat.tasks, function (task) {
                    task.filter = false;
                });
            });
        }
    };

    loadData();
}]);