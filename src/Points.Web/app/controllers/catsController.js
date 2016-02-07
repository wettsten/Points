'use strict';
app.controller('catsController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.loadCats = function () {
        $scope.cats = resourceService.get('categories');
        $timeout(function () {
            if ($scope.cats.length === 0) {
                $scope.addAlert('warning', 'No categories found');
            }
        }, 1000);
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
    });

    $scope.addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };

    $scope.loadCats();
}]);