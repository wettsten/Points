'use strict';
app.directive('filterCat', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/filterCat.html',
        replace: true,
        controller: 'filterCatController'
    };
}).controller('filterCatController', ['$scope', 'filterFactory', function ($scope, filterFactory) {

    $scope.filter = {
        isOpen: false,
        text: '',
        privateOnly: false
    };

    $scope.search = function() {
        filterFactory.setCatFilter({
            name: $scope.filter.text,
            isPrivate: $scope.filter.privateOnly
    });
    };

    $scope.clear = function () {
        $scope.filter.text = '';
        $scope.filter.privateOnly = false;
        $scope.search();
    };
}]);