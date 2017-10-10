var app = angular.module("cantinaApp", ['ui.bootstrap', 'ui.bootstrap.datetimepicker'])

.filter("convertToDate", function() {
    var re = /\/Date\(([0-9]*)\)\//;
    return function(x) {
        var m = x.match(re);
        if( m ) return new Date(parseInt(m[1]));
        else return null;
    };
})

.controller("cantinaCtrl", function ($scope, $http) {
    $scope.init = function () {
        $http.get('Base/GetUser').then(function (response) {
            $scope.User = response.data.user;
        });
    };

    $scope.logoff = function () {
        $http.delete('Base/Logout')
            .success(function (response) {
                window.location.href = 'Login';
            })
    }
});