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
}).controller('editCatController', ['$scope', 'catsService', 'authService', '$uibModal', function ($scope, catsService, authService, $uibModal) {

    $scope.editCat = {};

    $scope.isInEditMode = function() {
        return $scope.$parent.$parent.editCatId === $scope.cat.id;
    };

    $scope.isSomeoneElseInEditMode = function () {
        return $scope.$parent.$parent.editCatId !== '' && $scope.$parent.$parent.editCatId !== $scope.cat.id;
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

    $scope.startEdit = function () {
        for (var i = 0; i < $scope.$parent.cats.length; i++) {
            if ($scope.$parent.cats[i].id === $scope.cat.id) {
                $scope.editCat = angular.copy($scope.$parent.cats[i]);
                $scope.$parent.$parent.editCatId = $scope.cat.id;
                $scope.editForm.$show();
                break;
            }
        }
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.editForm.$setDirty();
            return "Name is required!";
        }
        $scope.editForm.$setPristine();
    };

    $scope.delete = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/confirmDelete.html',
            controller: 'confirmDeleteController',
            size: 'sm',
            resolve: {
                item: function () {
                    return {
                        name: $scope.cat.name,
                        id: $scope.cat.id
                    };
                }
            }
        });

        modalInstance.result.then(function (result) {
            if (result !== 'cancel') {
                catsService.deleteCat($scope.cat.id).then(
                    function (response) {
                        $scope.$parent.loadCats();
                    },
                    function(err) {
                        $scope.$parent.$parent.$parent.message = err.data.message;
                    });
            }
        });
    };
}]);