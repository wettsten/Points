'use strict';
app.directive('editCat', function () {
    return {
        scope: {
            cat: '=theCat',
            catInEdit: '=',
            addSuccess: '&',
            addError: '&'
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
        var name = $scope.editCat.name;
        resourceService.edit('categories',$scope.editCat).then(
            function (response) {
                $scope.clearEditData();
                $scope.addSuccess({ msg: "Category '{0}' successfully updated".format(name) });
            },
            function (err) {
                $scope.addError({ msg: err.data.message });
            }
        );
    };

    $scope.delete = function () {
        var name = $scope.cat.name;
        modalService.newModal('confirmDelete', { name: $scope.cat.name, id: $scope.cat.id }, 'sm',
            function (result) {
                resourceService.delete('categories', $scope.cat.id).then(
                    function (response) {
                        $scope.addSuccess({ msg: "Category '{0}' successfully deleted".format(name) });
                    },
                    function (err) {
                        $scope.addError({ msg: err.data.message });
                    }
                );
            }
        );
    };
}]);