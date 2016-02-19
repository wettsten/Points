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
}).controller('editCatController', ['$scope', 'resourceService', 'modalService', function ($scope, resourceService, modalService) {

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
        resourceService.edit('categories',$scope.editCat).then(
            function (response) {
                $scope.clearEditData();
                $scope.addAlert({ type: 'success', msg: 'Category successfully updated' });
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
            }
        );
    };

    $scope.delete = function () {
        modalService.newModal('confirmDelete', { name: $scope.cat.name, id: $scope.cat.id }, 'sm',
            function (result) {
                resourceService.delete('categories', $scope.cat.id).then(
                    function (response) {
                        $scope.addAlert({ type: 'success', msg: 'Category successfully deleted' });
                    },
                    function (err) {
                        $scope.addAlert({ type: 'danger', msg: err.data.message });
                    }
                );
            }
        );
    };
}]);