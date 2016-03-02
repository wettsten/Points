(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('catsController', catsController);

    catsController.$inject = ['$scope', 'resourceService', 'filterService', '$timeout'];

    function catsController($scope, resourceService, filterService, $timeout) {
        /* jshint validthis:true */
        var catsVm = this;

        catsVm.cats = [];
        catsVm.catInEdit = { id: '' };
        catsVm.catFilter = filterService.getCatFilter();
        catsVm.loadCats = loadCats;

        activate();

        function activate() {
            filterService.subscribe($scope, 'catFilter', function catFilterChanged() {
                catsVm.catFilter = filterService.getCatFilter();
            });

            resourceService.get('categories', function (data) {
                catsVm.cats = data;
                if (data.length === 0) {
                    catsVm.addWarning('No categories found');
                }
            });
        }

        function loadCats () {
            resourceService.get('categories');
        }
    }
})();
