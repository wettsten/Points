'use strict';
app.directive('editCat', function () {
    return {
        scope: {
            cat: '=theCat'
        },
        templateUrl: '/app/views/editCat.html',
        replace: true,
        controller: 'editCatController'
    };
}).controller('editCatController', ['$scope', 'catsService', 'authService', 'ngAuthSettings', function ($scope, catsService, authService, ngAuthSettings) {

    $scope.setDeleteIcon = function(cat, isActive) {
        cat.deleteIcon = isActive ? ngAuthSettings.icons.deleteActiveIcon : ngAuthSettings.icons.deleteIcon;
    };

    $scope.setEditIcon = function(cat, isActive) {
        cat.editIcon = isActive ? ngAuthSettings.icons.editActiveIcon : ngAuthSettings.icons.editIcon;
    };

    $scope.setCancelIcon = function(cat, isActive) {
        cat.cancelIcon = isActive ? ngAuthSettings.icons.cancelActiveIcon : ngAuthSettings.icons.cancelIcon;
    };

    $scope.setSaveIcon = function(cat, isActive) {
        cat.saveIcon = isActive ? ngAuthSettings.icons.saveActiveIcon : ngAuthSettings.icons.saveIcon;
    };

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
    };

    $scope.saveEdit = function () {
        if ($scope.editHasError('editName')) {
            $scope.$parent.message = 'Name is required!';
            return;
        }
        $scope.editCat.userId = authService.authentication.userId;
        catsService.editCat($scope.editCat).then(function (response) {
            $scope.clearEditData();
            $scope.$parent.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.startEdit = function (catId) {
        for (var i = 0; i < $scope.$parent.cats.length; i++) {
            if ($scope.$parent.cats[i].id === catId) {
                $scope.editCat = angular.copy($scope.$parent.cats[i]);
                $scope.$parent.$parent.editCatId = catId;
                break;
            }
        }
    };

    $scope.deleteCat = function (catId) {
        catsService.deleteCat(catId).then(function (response) {
            $scope.$parent.loadCats();
        },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.editHasError = function (field, validation) {
        if (validation) {
            return false;//$scope.editForm[field].$error[validation];
        }
        return false;//$scope.editForm[field].$invalid;
    };
}]);