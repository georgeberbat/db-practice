// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contact.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Contact _$ContactFromJson(Map<String, dynamic> json) => new Contact(
    id: json['_id'] as String,
    name: json['name'] as String,
    phoneNumbers: json['phoneNumbers'] as List<SavePhoneNumberDto>,
    email: json['email'] as String,
    address: json['address'] as String,
    groupIds: json['groupIds'] as List<String>,
    contactImage: json['contact_image'] as String);

abstract class _$ContactSerializerMixin {
  String get id;

  String get name;

  List<SavePhoneNumberDto> get phoneNumbers;

  String get email;

  String get address;

  List<String> groupIds;

  String get contactImage;

  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'name': name,
        'phoneNumbers': phoneNumbers,
        'email': email,
        'address': address,
        'contact_image': contactImage,
        'groupIds': groupIds
      };
}
