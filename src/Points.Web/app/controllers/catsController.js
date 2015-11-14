'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', 'ngAuthSettings', function ($scope, catsService, authService, ngAuthSettings) {

    $scope.editIconSrc = ngAuthSettings.icons.editIcon;
    $scope.deleteIconSrc = ngAuthSettings.icons.deleteIcon;

    $scope.setDeleteIcon = function(isActive) {
        $scope.deleteIconSrc = isActive ? ngAuthSettings.icons.deleteActiveIcon : ngAuthSettings.icons.deleteIcon;
    }

    $scope.setEditIcon = function (isActive) {
        $scope.editIconSrc = isActive ? ngAuthSettings.icons.editActiveIcon : ngAuthSettings.icons.editIcon;
    }

    $scope.selectedCat = {};
    $scope.cats = [];
    $scope.catData = {
        id: "",
        name: ""
    };
    $scope.message = '';

    $scope.clearData = function() {
        $scope.catData = {
            id: "",
            name: ""
        }
    };

    var loadCats = function() {
        catsService.getCats().then(function(results) {
            $scope.cats = results.data;
            $scope.clearData();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.add = function () {
        catsService.addCat($scope.catData).then(function (response) {
                loadCats();
            },
         function (err) {
             if (err.status == 409) {
                 $scope.message = 'Category already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    $scope.edit = function (catId) {
        catsService.editCat($scope.selectedCat).then(function (response) {
            loadCats();
        },
         function (err) {
             if (err.status == 409) {
                 $scope.message = 'Category already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    $scope.delete = function (catId) {
        catsService.deleteCat(catId).then(function (response) {
            loadCats();
        },
         function (err) {
             if (err.status == 409) {
                 $scope.message = 'Category already exists';
             } else {
                 $scope.message = err.status + ' ' + err.data;
             }
         });
    };

    loadCats();
}]);