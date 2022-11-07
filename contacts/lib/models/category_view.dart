class CategoryView {
  String text;
  String id;

  CategoryView(this.id, this.text);

  factory CategoryView.fromJson(Map<String, dynamic> json) => CategoryView(
    json["id"],
    json["name"]);
}