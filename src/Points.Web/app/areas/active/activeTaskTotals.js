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

    $scope.totals = {};
    $scope.hideCats = true;

    var loadTotals = function () {
        resourceService.get('activetotals');
    };

    resourceService.subscribe('activetotals', function (data) {
        $scope.totals = data;
        angular.forEach($scope.totals.categories, function(cat) {
            cat.hideTasks = true;
        });
    });

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