'use strict';
app.directive('imgButton', function() {
    return {
        scope: {
            type: '@btnType',
            disabled: '=ngDisabled'
        },
        templateUrl: '/app/views/directives/imgButton.html',
        replace: true,
        controller: 'imgButtonController'
    };
}).controller('imgButtonController', ['$scope', function ($scope) {

    $scope.icon = '';
    $scope.setIcon = function (isActive) {
        $scope.icon = isActive && !$scope.disabled ? 'Icons/' + $scope.type + 'active.png' : 'Icons/' + $scope.type + '.png';
    };
    $scope.setIcon(false);
}]);