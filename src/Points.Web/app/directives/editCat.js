'use strict';
app.directive('editCat', function () {
    return {
        scope: {
            cat: '=theCat'
        },
        templateUrl: '/app/views/directives/editCat.html',
        replace: true,
        controller: 'editCatController'
    };
}).controller('editCatController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.editCat = {};

    $scope.isInEditMode = function(catId) {
        return $scope.$parent.$parent.editCatId === catId;
    };

    $scope.isSomeoneElseInEditMode = function (catId) {
        return $scope.$parent.$parent.editCatId !== '' && $scope.$parent.$parent.editCatId !== catId;
    };

    $scope.clearEditData = function () {
        $scope.editCat = {};
        $scope.$parent.$parent.editCatId = '';
        $scope.editForm.$hide();
    };

    $scope.saveEdit = function () {
        $scope.editForm.$submit();
        if ($scope.editForm.$dirty) {
            $scope.editForm.$show();
            return;
        }
        $scope.editCat.userId = authService.authentication.userId;
        catsService.editCat($scope.editCat).then(function (response) {
            $scope.clearEditData();
            $scope.$parent.loadCats();
            },
         function (err) {
             $scope.$parent.$parent.$parent.message = err.data.message;
         });
    };

    $scope.startEdit = function (catId) {
        for (var i = 0; i < $scope.$parent.cats.length; i++) {
            if ($scope.$parent.cats[i].id === catId) {
                $scope.editCat = angular.copy($scope.$parent.cats[i]);
                $scope.$parent.$parent.editCatId = catId;
                $scope.editForm.$show();
                break;
            }
        }
    };

    $scope.deleteCat = function (catId) {
        catsService.deleteCat(catId).then(function (response) {
            $scope.$parent.loadCats();
        },
         function (err) {
             $scope.$parent.$parent.$parent.message = err.data.message;
         });
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.editForm.$setDirty();
            return "Name is required!";
        }
        $scope.editForm.$setPristine();
    };
}]);