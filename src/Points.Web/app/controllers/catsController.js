'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', 'ngAuthSettings', function ($scope, catsService, authService, ngAuthSettings) {

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

    $scope.editCat = {
        id: ""
    };
    $scope.cats = [];
    $scope.addCatData = {
        id: "",
        name: ""
    };
    $scope.message = '';

    $scope.hideData = function(catId) {
        return $scope.editCat.id === catId;
    };

    $scope.hideEditCancel = function() {
        return $scope.editCat.id.length > 0;
    };

    $scope.showSaveCancel = function(catId) {
        return $scope.editCat.id === catId;
    };

    $scope.clearData = function () {
        $scope.catData = {
            id: "",
            name: ""
        };
        $scope.editCat = {
            id: ""
        };
    };

    $scope.loadCats = function() {
        catsService.getCats().then(function(results) {
            $scope.cats = results.data;
            $scope.clearData();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.addCat = function () {
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.loadCats();
            },
         function (err) {
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.saveEdit = function () {
        catsService.editCat($scope.editCat).then(function (response) {
            $scope.loadCats();
        },
         function (err) {
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.startEdit = function (catId) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === catId) {
                $scope.editCat = {
                    id: $scope.cats[i].id,
                    name: $scope.cats[i].name
                };
                break;
            }
        }
    };

    $scope.cancelEdit = function () {
        $scope.editCat = {
            id: ""
        };
    };

    $scope.deleteCat = function (catId) {
        catsService.deleteCat(catId).then(function (response) {
            $scope.loadCats();
        },
         function (err) {
             $scope.message = err.status + ' ' + err.data.message;
         });
    };

    $scope.loadCats();
}]);