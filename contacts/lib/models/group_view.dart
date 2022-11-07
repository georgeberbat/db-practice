class GroupView {
  String text;
  String id;

  GroupView(this.id, this.text);

  factory GroupView.fromJson(Map<String, dynamic> json) => GroupView(
    json["id"],
    json["name"]);
}