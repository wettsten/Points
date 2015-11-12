'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.cats = [];

    catsService.getCats().then(function (results) {

        $scope.cats = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

}]);