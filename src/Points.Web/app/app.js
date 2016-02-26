
var app = angular.module('PointsApp', ['ngRoute', 'ngAnimate', 'LocalStorageModule', 'ui.bootstrap']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/active", {
        controller: "activeController",
        templateUrl: "/app/views/active.html"
    });

    $routeProvider.when("/planning", {
        controller: "planningController",
        templateUrl: "/app/views/planning.html"
    });

    $routeProvider.when("/cats", {
        controller: "catsController",
        templateUrl: "/app/views/cats.html"
    });

    $routeProvider.when("/tasks", {
        controller: "tasksController",
        templateUrl: "/app/views/tasks.html"
    });

    $routeProvider.when("/options", {
        controller: "optionsController",
        templateUrl: "/app/views/options.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var authServiceBase = 'http://points.wettsten.com/auth/';
var resourceServiceBase = 'http://points.wettsten.com/resources/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: authServiceBase,
    apiResourceBaseUri: resourceServiceBase,
    clientId: 'pointsprogram'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


