'use strict';
app.directive('activeTaskTotals', function () {
    return {
        scope: {
        },
        templateUrl: '/app/views/directives/activeTaskTotals.html',
        replace: true,
        controller: 'activeTaskTotalsController'
    };
}).controller('activeTaskTotalsController', ['$scope', 'resourceService', function ($scope, resourceService) {

    $scope.isSuccess = false;
    $scope.totals = {};
    $scope.user = {};
    $scope.hideCats = true;
    $scope.totalClass = 'active';

    var calculateTotalClass = function () {
        if ($scope.user.activeTargetPoints && $scope.totals.totalPoints) {
            var pct = $scope.totals.totalPoints * 100 / $scope.user.activeTargetPoints;
            if (pct >= 100) {
                $scope.totalClass = 'success';
                $scope.isSuccess = true;
            } else if (pct >= 50) {
                $scope.totalClass = 'warning';
            } else if (pct > 0) {
                $scope.totalClass = 'danger';
            } else {
                $scope.totalClass = 'active';
            }
        }
    };
    
    var calculateItemClass = function(item) {
        var pct = item.totalPoints * 100 / item.targetPoints;
        if (pct >= 100) {
            item.class = 'success';
        } else if (pct >= 50) {
            item.class = 'warning';
        } else {
            item.class = 'danger';
        }
    }

    var loadTotals = function () {
        resourceService.get('activetotals',  function (data) {
            $scope.totals = data;
            angular.forEach($scope.totals.categories, function(cat) {
                cat.hideTasks = true;
                calculateItemClass(cat);
                angular.forEach(cat.tasks, function(task) {
                    calculateItemClass(task);
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
        $scope.hideCats = !$scope.hideCats;
        angular.forEach($scope.totals.categories, function (cat) {
            cat.hideTasks = true;
        });
    };

    $scope.toggleTasks = function(cat) {
        cat.hideTasks = !cat.hideTasks;
    };

    loadTotals();
}]);