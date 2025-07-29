string[] names = ["Michael", "Pam", "Kim", "Dwight", "Angel", "Kevin", "Toby", "Creed"];

SectionTitle("Deffered execution");

var query1 = names.Where(name => name.EndsWith("m"));


var query2 = from name in names where name.EndsWith("m") select name;

