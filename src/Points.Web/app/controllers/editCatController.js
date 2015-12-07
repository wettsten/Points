'use strict';
app.controller('editCatController', ['$scope', 'catsService', 'authService', 'ngAuthSettings', function ($scope, catsService, authService, ngAuthSettings) {

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

    $scope.isInEditMode = function() {
        return $scope.editCat.id ? true : false;
    };

    $scope.amIInEditMode = function(catId) {
        return $scope.editCat.id === catId;
    };

    $scope.clearEditData = function () {
        $scope.editCat = {};
    };

    $scope.saveEdit = function () {
        $scope.editCat.userId = authService.authentication.userId;
        catsService.editCat($scope.editCat).then(function (response) {
            $scope.clearEditData();
            $scope.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.startEdit = function (catId) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === catId) {
                $scope.editCat = angular.copy($scope.cats[i]);
                break;
            }
        }
    };

    $scope.deleteCat = function (catId) {
        catsService.deleteCat(catId).then(function (response) {
            $scope.loadCats();
        },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };
}]);