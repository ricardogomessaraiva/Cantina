app.directive("dateMask", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attrs, ctrl) {
            var _formatDate = function (date) {
                date = date.replace(/[^0-9]+/g, '');
                console.log(date.length);
                if (date.length > 2) {
                    date = date.substring(0, 2) + "/" + date.substring(2);
                }

                if (date.length > 5) {
                    date = date.substring(0, 5) + "/" + date.substring(5, 9);
                }

                return date;
            };

            element.bind("keyup", function () {
                ctrl.$setViewValue(_formatDate(ctrl.$viewValue));
                ctrl.$render();
            })
        }
    };
});

app.directive('phoneMask', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {
            var _formatPhone = function (phone) {                
                phone = phone = phone.replace(/^[a-zA-Z]*$/, '')

                if (phone.length == 2)
                    phone = '(' + phone + ') ';
                if (phone.length == 9)
                    phone += '-';
                if (phone.length == 14) {
                    if (phone.substring(11, 10) == '-')
                        phone = '(' + phone.substring(1, 3) + ')' + phone.substring(4, 9) + phone.substring(11, 10) + phone.substring(8, 9) + phone.substring(11, 14);
                }
                if (phone.length == 15)
                    phone = '(' + phone.substring(1, 3) + ') ' + phone.substring(5, 11).replace('-', '') + '-' + phone.substring(11, 15);

                return phone;
            };

            element.bind('keyup', function () {
                ctrl.$setViewValue(_formatPhone(ctrl.$viewValue));
                ctrl.$render();
            })
        }
    };
})