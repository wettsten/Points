'use strict';
app.directive('newCatRow', function() {
    return {
        scope: {},
        templateUrl: '/app/views/newCatRow.html',
        replace: true,
        controller: 'newCatRowController'
    };
}).controller('newCatRowController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
        $scope.addForm.$show();
    };

    $scope.addCat = function () {
        $scope.addForm.$submit();
        if ($scope.addForm.$dirty) {
            return;
        }
        $scope.addCatData.userId = authService.authentication.userId;
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.clearAddData();
            $scope.$parent.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.addForm.$setDirty();
            return "Name is required!";
        }
        $scope.addForm.$setPristine();
    };
}]);