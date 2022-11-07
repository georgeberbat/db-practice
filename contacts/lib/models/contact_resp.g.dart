// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contact_resp.dart';


// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ContactResponse _$ContactResponseFromJson(Map<String, dynamic> json) => new ContactResponse(
    id: json['id'] as String,
    name: json['name'] as String,
    phoneNumbers: json['phoneNumbers'] as List<ResponsePhoneNumberDto>,
    email: json['email'] as String,
    address: json['address'] as String,
    groups: json['groups'] as List<GroupView>,
    contactImage: json['contact_image'] as String);

abstract class _$ContactResponseSerializerMixin {
  String get id;

  String get name;

  List<ResponsePhoneNumberDto> get phoneNumbers;

  String get email;

  String get address;

  List<GroupView> groups;

  String get contactImage;

  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'name': name,
        'phoneNumbers': phoneNumbers,
        'email': email,
        'address': address,
        'contact_image': contactImage,
        'groupIds': groups
      };
}
