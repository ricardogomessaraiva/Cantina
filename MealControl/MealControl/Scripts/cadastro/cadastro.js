app.controller('cadastroCtrl', function ($scope, $http, factory) {
    angular.element('#cadastro-usuario').addClass("start active open");
    $scope.phones = [{ Number: null }];
    $scope.students = factory.createStudent;
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
        $scope.Parent.Phone = angular.copy($scope.phones)
        $scope.Parent.Students = angular.copy($scope.students);

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
        $scope.phones.push({ Number: '' });
    }

    $scope.removePhone = function (phone) {
        var pos = $scope.phones.indexOf(phone);
        $scope.phones.splice(pos, 1);
    }

    $scope.addStudent = function () {
        var student = factory.createStudent;
        $scope.students.push(student);
    }

    $scope.removeStudent = function (i) {
        $scope.students.splice(i, 1);
    }

});