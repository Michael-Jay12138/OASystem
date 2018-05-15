(function () {
    var app = angular.module("atThePrivilege", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/privilege/list", { templateUrl: "../client/views/privilege/list.html" })
            .otherwise({ redirectTo: "/privilege/list" });
    }
    app.config(config);
    app.constant("privilegeApiUrl", "../privilege/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());