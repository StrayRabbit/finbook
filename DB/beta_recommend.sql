/*
 Navicat Premium Data Transfer

 Source Server         : 134.175.49.139
 Source Server Type    : MySQL
 Source Server Version : 80013
 Source Host           : 134.175.49.139:3306
 Source Schema         : beta_recommend

 Target Server Type    : MySQL
 Target Server Version : 80013
 File Encoding         : 65001

 Date: 21/11/2019 21:21:19
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for ProjectRecommends
-- ----------------------------
DROP TABLE IF EXISTS `ProjectRecommends`;
CREATE TABLE `ProjectRecommends`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `FromUserId` int(11) NOT NULL,
  `FromUserName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `FromUserAvatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `RecommednType` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `ProjectAvatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Company` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Introduction` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Tags` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `FinStage` int(11) NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  `RecommendTime` datetime(0) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProjectRecommends
-- ----------------------------
INSERT INTO `ProjectRecommends` VALUES (1, 1, 2, '紫霞', NULL, 2, -2147482647, NULL, 'jessetalk', '公司介绍5', NULL, 0, '2019-01-07 20:21:13', '2019-01-08 09:41:50');
INSERT INTO `ProjectRecommends` VALUES (2, 1, 2, '紫霞', NULL, 2, -2147482647, NULL, 'jessetalk', '公司介绍', NULL, 0, '2019-01-08 10:34:16', '2019-01-08 10:44:38');
INSERT INTO `ProjectRecommends` VALUES (3, 1, 2, '紫霞', NULL, 2, -2147482646, NULL, 'jessetalk', '公司介绍', NULL, 0, '2019-01-08 10:45:55', '2019-01-08 10:50:06');

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
INSERT INTO `__EFMigrationsHistory` VALUES ('20190104005318_init', '2.1.2-rtm-30932');
INSERT INTO `__EFMigrationsHistory` VALUES ('20190107092255_initcapTables', '2.1.2-rtm-30932');

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
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.received
-- ----------------------------
INSERT INTO `cap.received` VALUES (1, 'finbook.projectapi.projectcreated', 'cap.queue.recommend.api', '{\"Id\":\"5c3335185e8fb35c44ce020d\",\"Timestamp\":\"2019-01-07T19:16:40.207+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482647,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍3\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-07T19:16:39.406+08:00\\\"}\",\"CallbackName\":null}', 0, '2019-01-07 19:20:53', '2019-01-08 19:21:05', 'Succeeded');
INSERT INTO `cap.received` VALUES (2, 'finbook.projectapi.projectcreated', 'cap.queue.recommend.api', '{\"Id\":\"5c3336705e8fb35c44ce020e\",\"Timestamp\":\"2019-01-07T19:22:24.918+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482646,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍4\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-07T19:22:24.224+08:00\\\"}\",\"CallbackName\":null,\"ExceptionMessage\":{\"Source\":\"DotNetCore.CAP\",\"Message\":\"message can not be found subscriber, Message:name:finbook.projectapi.projectcreated, group:cap.queue.recommend.api, content:{\\\"Id\\\":\\\"5c3336705e8fb35c44ce020e\\\",\\\"Timestamp\\\":\\\"2019-01-07T19:22:24.918+08:00\\\",\\\"Content\\\":\\\"{\\\\\\\"ProjectId\\\\\\\":-2147482646,\\\\\\\"UserId\\\\\\\":2,\\\\\\\"Company\\\\\\\":\\\\\\\"jessetalk\\\\\\\",\\\\\\\"FinStage\\\\\\\":0,\\\\\\\"Introduction\\\\\\\":\\\\\\\"公司介绍4\\\\\\\",\\\\\\\"ProjectAvatar\\\\\\\":null,\\\\\\\"Tags\\\\\\\":null,\\\\\\\"CreateTime\\\\\\\":\\\\\\\"2019-01-07T19:22:24.224+08:00\\\\\\\"}\\\",\\\"CallbackName\\\":null,\\\"ExceptionMessage\\\":{\\\"Source\\\":\\\"DotNetCore.CAP\\\",\\\"Message\\\":\\\"Object reference not set to an instance of an object.\\\",\\\"InnerMessage\\\":\\\"Object reference not set to an instance of an object.\\\"}},\\r\\n see: https://github.com/dotnetcore/CAP/issues/63\",\"InnerMessage\":null}}', 51, '2019-01-07 19:26:55', NULL, 'Scheduled');
INSERT INTO `cap.received` VALUES (3, 'finbook.projectapi.projectcreated', 'cap.queue.recommend.api', '{\"Id\":\"5c33443a5e8fb364f07aca00\",\"Timestamp\":\"2019-01-07T20:21:14.045+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482647,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍5\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-07T20:21:13.214+08:00\\\"}\",\"CallbackName\":null,\"ExceptionMessage\":{\"Source\":\"DotNetCore.CAP\",\"Message\":\"Object reference not set to an instance of an object.\",\"InnerMessage\":\"Object reference not set to an instance of an object.\"}}', 11, '2019-01-07 20:25:52', '2019-01-09 09:41:52', 'Succeeded');
INSERT INTO `cap.received` VALUES (4, 'finbook.projectapi.projectcreated', 'cap.queue.recommend.api', '{\"Id\":\"5c340c295e8fb335d4102c4e\",\"Timestamp\":\"2019-01-08T10:34:17.2294002+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482647,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-08T10:34:16.3794002+08:00\\\"}\",\"CallbackName\":null,\"ExceptionMessage\":{\"Source\":\"DotNetCore.CAP\",\"Message\":\"Object reference not set to an instance of an object.\",\"InnerMessage\":\"Object reference not set to an instance of an object.\"}}', 5, '2019-01-08 10:39:01', '2019-01-09 10:44:48', 'Succeeded');
INSERT INTO `cap.received` VALUES (5, 'finbook.projectapi.projectcreated', 'cap.queue.recommend.api', '{\"Id\":\"5c340ee45e8fb335d4102c4f\",\"Timestamp\":\"2019-01-08T10:45:56.0074002+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482646,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-08T10:45:55.2804002+08:00\\\"}\",\"CallbackName\":null}', 1, '2019-01-08 10:50:04', '2019-01-09 10:50:07', 'Succeeded');

SET FOREIGN_KEY_CHECKS = 1;
