(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('editCat', editCat);
    
    function editCat() {
        var directive = {
            restrict: 'EA',
            scope: {
                cat: '=theCat',
                catInEdit: '=',
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/categories/editCat.html',
            controller: 'editCatController',
            controllerAs: 'editCatVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('editCatController', editCatController);

    editCatController.$inject = ['$scope', 'resourceService', 'modalService'];

    function editCatController($scope, resourceService, modalService) {
        /* jshint validthis:true */
        var editCatVm = this;

        editCatVm.editCat = {};
        editCatVm.isInEditMode = isInEditMode;
        editCatVm.clearEditData = clearEditData;
        editCatVm.startEdit = startEdit;
        editCatVm.saveEdit = saveEdit;
        editCatVm.delete = deleteCat;

        activate();

        function activate() {
            $scope.$watch('editCatVm.catInEdit.id', function () {
                if (editCatVm.catInEdit.id !== '' && editCatVm.catInEdit.id !== editCatVm.cat.id) {
                    editCatVm.editCat = {};
                }
            });
        }

        function isInEditMode() {
            return editCatVm.catInEdit.id === editCatVm.cat.id;
        }

        function clearEditData() {
            editCatVm.editCat = {};
            editCatVm.catInEdit.id = '';
        }

        function startEdit () {
            editCatVm.editCat = angular.copy(editCatVm.cat);
            editCatVm.catInEdit.id = editCatVm.cat.id;
        }

        function saveEdit () {
            var name = editCatVm.editCat.name;
            resourceService.edit('categories', editCatVm.editCat).then(
                function (response) {
                    clearEditData();
                    editCatVm.addSuccess({ msg: "Category '{0}' successfully updated".format(name) });
                },
                function (err) {
                    editCatVm.addError({ msg: err.data.message });
                }
            );
        }

        function deleteCat() {
            var name = editCatVm.cat.name;
            modalService.newModal('confirmDelete', 'common', { name: editCatVm.cat.name, id: editCatVm.cat.id }, 'sm',
                function (result) {
                    resourceService.remove('categories', editCatVm.cat.id).then(
                        function (response) {
                            editCatVm.addSuccess({ msg: "Category '{0}' successfully deleted".format(name) });
                        },
                        function (err) {
                            editCatVm.addError({ msg: err.data.message });
                        }
                    );
                }
            );
        }
    }

})();