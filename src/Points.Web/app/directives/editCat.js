'use strict';
app.directive('editCat', function () {
    return {
        scope: {
            cat: '=theCat',
            catInEdit: '=',
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/editCat.html',
        replace: true,
        controller: 'editCatController'
    };
}).controller('editCatController', ['$scope', 'catsService', 'authService', '$uibModal', function ($scope, catsService, authService, $uibModal) {

    $scope.editCat = {};

    $scope.isInEditMode = function() {
        return $scope.catInEdit.id === $scope.cat.id;
    };

    $scope.$watch('catInEdit.id', function () {
        if ($scope.catInEdit.id !== '' && $scope.catInEdit.id !== $scope.cat.id) {
            $scope.editCat = {};
        }
    });

    $scope.clearEditData = function () {
        $scope.editCat = {};
        $scope.catInEdit.id = '';
    };

    $scope.startEdit = function () {
        $scope.editCat = angular.copy($scope.cat);
        $scope.catInEdit.id = $scope.cat.id;
    };

    $scope.saveEdit = function () {
        $scope.editCat.userId = authService.authentication.userId;
        catsService.editCat($scope.editCat).then(
            function (response) {
                $scope.clearEditData();
                $scope.$emit('refreshCats');
                $scope.addAlert({ type: 'success', msg: 'Category successfully updated' });
            },
             function (err) {
                 $scope.addAlert({ type: 'danger', msg: err.data.message });
             });
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
                        $scope.$emit('refreshCats');
                        $scope.addAlert({ type: 'success', msg: 'Category successfully deleted' });
                    },
                    function (err) {
                        $scope.addAlert({ type: 'danger', msg: err.data.message });
                    });
            }
        });
    };
}]);