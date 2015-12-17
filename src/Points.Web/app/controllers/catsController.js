'use strict';
app.controller('catsController', ['$scope', 'catsService', 'filterFactory', '$timeout', function ($scope, catsService, filterFactory, $timeout) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    $scope.loadCats = function() {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
        }, function (err) {
            $scope.addAlert('danger', err.statusText);
        });
    };

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.$on('refreshCats', function() {
        $scope.loadCats();
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