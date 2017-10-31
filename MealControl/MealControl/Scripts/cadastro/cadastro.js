app.controller('cadastroCtrl', function ($scope, $http, factory) {
    angular.element('#cadastro-usuario').addClass("start active open");
    $scope.phonesList = [{ Number: null }];
    $scope.studentsList = [{ Name: null, BirthDate: null, Period: null }];
    $scope.Parent = factory.createParent;
    $scope.retypeEmail = null;

    $scope.load = function () {
        $http.get('Cadastro/Load').then(success);
        function success(response) {
            $scope.periods = response.data.period;
        };
    }

    $scope.save = function () {
        $scope.errors = [];
        $scope.Parent.Phone = angular.copy($scope.phonesList)
        $scope.Parent.Students = angular.copy($scope.studentsList);

        $scope.Parent.Phone.forEach(function (phone, i) {
            if (!phone.Number)
                $scope.Parent.Phone.splice(i, 1);
        });

        $scope.Parent.Students.forEach(function (student, i) {
            if (!student.Name)
                $scope.Parent.Students.splice(i, 1);
            else if (!student.BirthDate)
                $scope.Parent.Students.splice(i, 1);
            else if (!student.Period)
                $scope.Parent.Students.splice(i, 1);
        });
        
        $http.post('Cadastro/Save', { parent: $scope.Parent }).then(success, error);
        function success(response) {
            if (response.status == 201)//CREATED
                return window.location.href = 'Confirmacao';

            for (var i in response.data) {
                $scope.errors.push(response.data[i].ErrorMessage);
            }
        };
        function error(response) {
            $scope.errors = new Array(response.statusText);
        };
    };

    $scope.addPhone = function () {
        $scope.phonesList.push({ Number: '' });
    }

    $scope.removePhone = function (phone) {
        var pos = $scope.phonesList.indexOf(phone);
        $scope.phonesList.splice(pos, 1);
    }

    $scope.addStudent = function () {
        $scope.studentsList.push({ Name: null, BirthDate: null, Period: null });
    }

    $scope.removeStudent = function (i) {
        $scope.students.splice(i, 1);
    }

});