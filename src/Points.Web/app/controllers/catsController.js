'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', 'filterFactory', function ($scope, catsService, authService, filterFactory) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    $scope.loadCats = function() {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
        }, function (err) {
            $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
    };

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.addAlert = function (type,msg) {
        $scope.alerts.push({ type: type, msg: msg });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.loadCats();
}]);