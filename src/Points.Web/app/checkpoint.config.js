(function () {
    'use strict';

    angular
        .module('cp.core')
        .config(config);

    config.$inject = ['$routeProvider', '$httpProvider'];

    function config($routeProvider, $httpProvider) {
        $routeProvider
            .when("/home", {
                controller: "homeController",
                templateUrl: "/app/views/home.html"
            })
            .when("/login", {
                controller: "loginController",
                templateUrl: "/app/views/login.html"
            })
            .when("/signup", {
                controller: "signupController",
                templateUrl: "/app/views/signup.html"
            })
            .when("/active", {
                controller: "activeController",
                templateUrl: "/app/views/active.html"
            })
            .when("/planning", {
                controller: "planningController",
                templateUrl: "/app/views/planning.html"
            })
            .when("/cats", {
                controller: "catsController",
                templateUrl: "/app/views/cats.html"
            })
            .when("/tasks", {
                controller: "tasksController",
                templateUrl: "/app/views/tasks.html"
            })
            .when("/options", {
                controller: "optionsController",
                templateUrl: "/app/views/options.html"
            })
            .when("/associate", {
                controller: "associateController",
                templateUrl: "/app/views/associate.html"
            })
            .otherwise({ redirectTo: "/home" });

        $httpProvider.interceptors.push('authInterceptorService');
    }

})();
