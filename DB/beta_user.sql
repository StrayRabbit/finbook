/*
 Navicat Premium Data Transfer

 Source Server         : 134.175.49.139
 Source Server Type    : MySQL
 Source Server Version : 80013
 Source Host           : 134.175.49.139:3306
 Source Schema         : beta_user

 Target Server Type    : MySQL
 Target Server Version : 80013
 File Encoding         : 65001

 Date: 21/11/2019 21:21:35
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for User
-- ----------------------------
DROP TABLE IF EXISTS `User`;
CREATE TABLE `User`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Address` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Avatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `City` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `CityId` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Company` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Email` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Gender` tinyint(4) NOT NULL,
  `Name` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `NameCard` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Phone` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Province` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `ProvinceId` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Title` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of User
-- ----------------------------
INSERT INTO `User` VALUES (1, '五指山', NULL, '香河', NULL, '斧头帮', NULL, 1, '至尊宝', NULL, '15931158885', NULL, NULL, '斧头帮帮主');
INSERT INTO `User` VALUES (2, '日月神灯', NULL, NULL, NULL, '日月神灯', NULL, 0, '紫霞', NULL, '15097369789', NULL, NULL, 'title');
INSERT INTO `User` VALUES (8, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, '15931158888', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for UserBPFile
-- ----------------------------
DROP TABLE IF EXISTS `UserBPFile`;
CREATE TABLE `UserBPFile`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AppUserId` int(11) NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  `FileName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `FormatFilePath` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `OriginFilePath` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for UserProperty
-- ----------------------------
DROP TABLE IF EXISTS `UserProperty`;
CREATE TABLE `UserProperty`  (
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `AppUserId` int(11) NOT NULL,
  `Value` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Text` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Key`, `AppUserId`, `Value`) USING BTREE,
  INDEX `IX_UserProperty_AppUserId`(`AppUserId`) USING BTREE,
  CONSTRAINT `FK_UserProperty_User_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for UserTag
-- ----------------------------
DROP TABLE IF EXISTS `UserTag`;
CREATE TABLE `UserTag`  (
  `UserId` int(11) NOT NULL,
  `Tag` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  PRIMARY KEY (`UserId`, `Tag`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO `__EFMigrationsHistory` VALUES ('20181024065928_init', '2.0.3-rtm-10026');

-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
DROP TABLE IF EXISTS `cap.published`;
CREATE TABLE `cap.published`  (
  `Id` int(127) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Retries` int(11) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.published
-- ----------------------------
INSERT INTO `cap.published` VALUES (4, 'finbook_userapi_userprofilechanged', '{\"Id\":\"5c340c1b5e8fb34e48dff9fd\",\"Timestamp\":\"2019-01-08T10:34:03.1284002+08:00\",\"Content\":\"{\\\"UserId\\\":2,\\\"Avatar\\\":null,\\\"Company\\\":null,\\\"Title\\\":\\\"title\\\",\\\"Name\\\":\\\"紫霞\\\"}\",\"CallbackName\":null}', 0, '2019-01-08 10:34:03', '2019-01-08 11:38:44', 'Succeeded');

-- ----------------------------
-- Table structure for cap.received
-- ----------------------------
DROP TABLE IF EXISTS `cap.received`;
CREATE TABLE `cap.received`  (
  `Id` int(127) NOT NULL AUTO_INCREMENT,
  `Name` varchar(400) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Group` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Retries` int(11) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
