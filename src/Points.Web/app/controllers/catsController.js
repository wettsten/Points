'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', 'ngAuthSettings', function ($scope, catsService, authService, ngAuthSettings) {
    
    $scope.setDeleteIcon = function(cat,isActive) {
        cat.deleteIcon = isActive ? ngAuthSettings.icons.deleteActiveIcon : ngAuthSettings.icons.deleteIcon;
    }

    $scope.setEditIcon = function (cat,isActive) {
        cat.editIcon = isActive ? ngAuthSettings.icons.editActiveIcon : ngAuthSettings.icons.editIcon;
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