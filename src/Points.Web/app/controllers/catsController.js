'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.cats = [];
    $scope.catData = {
        name: ""
    };
    $scope.message = '';

    var loadCats = function() {
        catsService.getCats().then(function(results) {
            $scope.cats = results.data;
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.add = function () {

        catsService.addCat($scope.catData).then(function (response) {
                $scope.catData.name = '';
                loadCats();
            },
         function (err) {
             $scope.message = err.error_description;
         });
    };

    loadCats();
}]);