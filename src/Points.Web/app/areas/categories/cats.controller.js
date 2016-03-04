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
        catsVm.refreshCats = refreshCats;

        activate();

        function activate() {
            filterService.subscribe($scope, 'catFilter', getCatFilter);
            resourceService.get('categories', getCats);
        }

        function getCatFilter() {
            catsVm.catFilter = filterService.getCatFilter();
        }

        function getCats(data) {
            catsVm.cats = data;
            if (data.length === 0) {
                catsVm.addWarning('No categories found');
            }
        }

        function refreshCats () {
            resourceService.get('categories');
        }
    }
})();
