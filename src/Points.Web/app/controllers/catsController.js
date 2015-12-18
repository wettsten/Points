'use strict';
app.controller('catsController', ['$scope', 'catsService', 'filterFactory', '$timeout', '$q', function ($scope, catsService, filterFactory, $timeout, $q) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    $scope.loadCats = catsService.getCats().then(
        function (results) {
            return results.data;
    });

    $scope.loadData = function () {
        $q.all([$scope.loadCats]).then(
            function (data) {
                $scope.cats = data[0];
        });
    };

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.$on('refreshCats', function() {
        $scope.loadData();
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

    $scope.loadData();
}]);