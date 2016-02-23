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
    });

    loadTotals();
}]);