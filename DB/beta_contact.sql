/*
 Navicat Premium Data Transfer

 Source Server         : 134.175.49.139
 Source Server Type    : MySQL
 Source Server Version : 80013
 Source Host           : 134.175.49.139:3306
 Source Schema         : beta_contact

 Target Server Type    : MySQL
 Target Server Version : 80013
 File Encoding         : 65001

 Date: 21/11/2019 21:20:50
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.received
-- ----------------------------
INSERT INTO `cap.received` VALUES (2, 'finbook_userapi_userprofilechanged', 'cap.queue.contact.api', '{\"Id\":\"5c32eab95e8fb35728768428\",\"Timestamp\":\"2019-01-07T13:59:21.3+08:00\",\"Content\":\"{\\\"UserId\\\":2,\\\"Avatar\\\":null,\\\"Company\\\":null,\\\"Title\\\":\\\"title2\\\",\\\"Name\\\":\\\"紫霞\\\"}\",\"CallbackName\":null}', 0, '2019-01-07 14:03:35', '2019-01-08 14:03:36', 'Succeeded');
INSERT INTO `cap.received` VALUES (3, 'finbook_userapi_userprofilechanged', 'cap.queue.contact.api', '{\"Id\":\"5c340c1b5e8fb34e48dff9fd\",\"Timestamp\":\"2019-01-08T10:34:03.1284002+08:00\",\"Content\":\"{\\\"UserId\\\":2,\\\"Avatar\\\":null,\\\"Company\\\":null,\\\"Title\\\":\\\"title\\\",\\\"Name\\\":\\\"紫霞\\\"}\",\"CallbackName\":null}', 0, '2019-01-08 10:38:44', '2019-01-09 10:38:45', 'Succeeded');

SET FOREIGN_KEY_CHECKS = 1;
