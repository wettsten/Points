﻿(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('newCat', newCat);
    
    function newCat() {
        var directive = {
            restrict: 'EA',
            scope: {
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/categories/newCat.html',
            controller: 'newCatController',
            controllerAs: 'newCatVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('newCatController', newCatController);

    newCatController.$inject = ['resourceService'];

    function newCatController(resourceService) {
        /* jshint validthis:true */
        var newCatVm = this;

        newCatVm.addCatData = {};
        newCatVm.clearAddData = clearAddData;
        newCatVm.addCat = addCat;

        activate();

        function activate() { }

        function clearAddData () {
            newCatVm.addCatData = {};
        }

        function addCat () {
            resourceService.add('categories', newCatVm.addCatData).then(
                function (response) {
                    var name = newCatVm.addCatData.name;
                    clearAddData();
                    newCatVm.addSuccess({ msg: "Category '{0}' successfully added".format(name) });
                },
                function (err) {
                    newCatVm.addError({ msg: err.data.message });
                }
            );
        }
    }

})();