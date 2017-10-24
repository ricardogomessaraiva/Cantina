app.factory('factory', function () {
    var _parent = {
        UserName: null,
        Password: null,
        Email: null,
        Name: null,
        CreatedAt: null,
        ModifiedAt: null,
        Status: null,
        Phone: { Id: 0, Number: null },
        Students: null,
        Status: null
    };

    var _student = {
        Name: null,
        BirthDate: null,
        Period: null
    }

    return {
        createParent: _parent,
        createStudent: _student
    };
})