/*
 Navicat Premium Data Transfer

 Source Server         : 134.175.49.139
 Source Server Type    : MySQL
 Source Server Version : 80013
 Source Host           : 134.175.49.139:3306
 Source Schema         : beta_project

 Target Server Type    : MySQL
 Target Server Version : 80013
 File Encoding         : 65001

 Date: 21/11/2019 21:21:01
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for ProjectContributors
-- ----------------------------
DROP TABLE IF EXISTS `ProjectContributors`;
CREATE TABLE `ProjectContributors`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `UserName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Avatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `IsCloser` bit(1) NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  `ContributorType` int(11) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ProjectContributors_ProjectId`(`ProjectId`) USING BTREE,
  CONSTRAINT `FK_ProjectContributors_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for ProjectProperties
-- ----------------------------
DROP TABLE IF EXISTS `ProjectProperties`;
CREATE TABLE `ProjectProperties`  (
  `ProjectId` int(11) NOT NULL,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Text` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`ProjectId`, `Key`, `Value`) USING BTREE,
  CONSTRAINT `FK_ProjectProperties_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for ProjectViewers
-- ----------------------------
DROP TABLE IF EXISTS `ProjectViewers`;
CREATE TABLE `ProjectViewers`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `UserName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Avatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ProjectViewers_ProjectId`(`ProjectId`) USING BTREE,
  CONSTRAINT `FK_ProjectViewers_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for ProjectVisibleRules
-- ----------------------------
DROP TABLE IF EXISTS `ProjectVisibleRules`;
CREATE TABLE `ProjectVisibleRules`  (
  `ProjectId` int(11) NOT NULL AUTO_INCREMENT,
  `Visible` bit(1) NOT NULL,
  `Tags` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `ProjectId1` int(11) NOT NULL,
  PRIMARY KEY (`ProjectId`) USING BTREE,
  INDEX `IX_ProjectVisibleRules_ProjectId1`(`ProjectId1`) USING BTREE,
  CONSTRAINT `FK_ProjectVisibleRules_Projects_ProjectId1` FOREIGN KEY (`ProjectId1`) REFERENCES `Projects` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Projects
-- ----------------------------
DROP TABLE IF EXISTS `Projects`;
CREATE TABLE `Projects`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `UserName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Company` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Introduction` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Avatar` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `OriginBPFile` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `FormatBPFile` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `ShowSecurityInfo` bit(1) NOT NULL,
  `ProvinceId` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `ProvinceName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `CityId` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `CityName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `AreaId` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `AreaName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `RegisterTime` datetime(0) NOT NULL,
  `FinPercentage` int(11) NOT NULL,
  `FinStage` int(11) NOT NULL,
  `FinMoney` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Income` int(11) NOT NULL,
  `Revenue` int(11) NOT NULL,
  `Valuation` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Tags` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `BrokerageOption` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `SourceId` int(11) NOT NULL,
  `ReferenceId` int(11) NOT NULL,
  `UpdateTime` datetime(0) NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 23 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Projects
-- ----------------------------
INSERT INTO `Projects` VALUES (3, 2, NULL, 'jessetalk', '公司介绍', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (4, 2, NULL, 'jessetalk', '公司介绍', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (5, 2, NULL, 'jessetalk', '公司介绍1', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (6, 2, NULL, 'jessetalk', '公司介绍2', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (7, 2, NULL, 'jessetalk', '公司介绍3', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (8, 2, NULL, 'jessetalk', '公司介绍4', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (9, 2, NULL, 'jessetalk', '公司介绍5', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (10, 2, NULL, 'jessetalk', '公司介绍6', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (11, 2, NULL, 'jessetalk', '公司介绍7', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (12, 2, NULL, 'jessetalk', '公司介绍8', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (13, 2, NULL, 'jessetalk', '公司介绍9', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (14, 2, NULL, 'jessetalk', '公司介绍10', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (15, 2, NULL, 'jessetalk', '公司介绍11', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (16, 2, NULL, 'jessetalk', '公司介绍', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (17, 2, NULL, 'jessetalk', '公司介绍1', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (18, 2, NULL, 'jessetalk', '公司介绍2', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (19, 2, NULL, 'jessetalk', '公司介绍3', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (20, 2, NULL, 'jessetalk', '公司介绍4', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (21, 2, NULL, 'jessetalk', '公司介绍5', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (22, 2, NULL, 'jessetalk', '公司介绍', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');
INSERT INTO `Projects` VALUES (23, 2, NULL, 'jessetalk', '公司介绍', NULL, NULL, NULL, b'0', NULL, NULL, NULL, NULL, NULL, NULL, '0001-01-01 00:00:00', 0, 0, NULL, 0, 0, NULL, NULL, NULL, 0, 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00');

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
INSERT INTO `__EFMigrationsHistory` VALUES ('20181223013016_init', '2.2.0-rtm-35687');

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
) ENGINE = InnoDB AUTO_INCREMENT = 20 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.published
-- ----------------------------
INSERT INTO `cap.published` VALUES (19, 'finbook.projectapi.projectcreated', '{\"Id\":\"5c340c295e8fb335d4102c4e\",\"Timestamp\":\"2019-01-08T10:34:17.2294002+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482647,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-08T10:34:16.3794002+08:00\\\"}\",\"CallbackName\":null}', 0, '2019-01-08 10:34:17', '2019-01-08 11:39:01', 'Succeeded');
INSERT INTO `cap.published` VALUES (20, 'finbook.projectapi.projectcreated', '{\"Id\":\"5c340ee45e8fb335d4102c4f\",\"Timestamp\":\"2019-01-08T10:45:56.0074002+08:00\",\"Content\":\"{\\\"ProjectId\\\":-2147482646,\\\"UserId\\\":2,\\\"Company\\\":\\\"jessetalk\\\",\\\"FinStage\\\":0,\\\"Introduction\\\":\\\"公司介绍\\\",\\\"ProjectAvatar\\\":null,\\\"Tags\\\":null,\\\"CreateTime\\\":\\\"2019-01-08T10:45:55.2804002+08:00\\\"}\",\"CallbackName\":null}', 0, '2019-01-08 10:45:56', '2019-01-08 11:50:03', 'Succeeded');

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
