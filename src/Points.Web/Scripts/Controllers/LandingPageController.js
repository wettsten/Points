var LandingPageController = function ($scope) {
    $scope.models = {
        helloAngular: 'I work!'
    };
    $scope.navbarProperties = {
        isCollapsed: true
    };

    $scope.$on('event:google-plus-signin-success', function (event, authResult) {
        // User successfully authorized the G+ App!
        console.log('Signed in!');
    });
    $scope.$on('event:google-plus-signin-failure', function (event, authResult) {
        // User has not authorized the G+ App!
        console.log('Not signed into Google Plus.');
    });
}

// The $inject property of every controller (and pretty much every other type of object in Angular) needs to be a string array equal to the controllers arguments, only as strings
LandingPageController.$inject = ['$scope'];