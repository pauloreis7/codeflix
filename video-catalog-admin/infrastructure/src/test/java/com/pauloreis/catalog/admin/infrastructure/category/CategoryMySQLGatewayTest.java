package com.pauloreis.catalog.admin.infrastructure.category;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.infrastructure.MySQLGatewayTest;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryJpaEntity;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

@MySQLGatewayTest
public class CategoryMySQLGatewayTest {

  @Autowired
  private CategoryMySQLGateway categoryGateway;

  @Autowired
  private CategoryRepository categoryRepository;

  @Test
  public void givenAValidCategory_whenCallsCreate_shouldReturnANewCategory() {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var aCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    Assertions.assertEquals(0, categoryRepository.count());

    final var actualCategory = categoryGateway.create(aCategory);

    Assertions.assertEquals(1, categoryRepository.count());

    Assertions.assertEquals(aCategory.getId(), actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualCategory.getCreatedAt());
    Assertions.assertEquals(aCategory.getUpdatedAt(), actualCategory.getUpdatedAt());
    Assertions.assertEquals(aCategory.getDeletedAt(), actualCategory.getDeletedAt());
    Assertions.assertNull(actualCategory.getDeletedAt());

    final var actualEntity = categoryRepository.findById(aCategory.getId().getValue()).get();

    Assertions.assertEquals(aCategory.getId().getValue(), actualEntity.getId());
    Assertions.assertEquals(expectedName, actualEntity.getName());
    Assertions.assertEquals(expectedDescription, actualEntity.getDescription());
    Assertions.assertEquals(expectedIsActive, actualEntity.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualEntity.getCreatedAt());
    Assertions.assertEquals(aCategory.getUpdatedAt(), actualEntity.getUpdatedAt());
    Assertions.assertEquals(aCategory.getDeletedAt(), actualEntity.getDeletedAt());
    Assertions.assertNull(actualEntity.getDeletedAt());
  }

  @Test
  public void givenAValidCategory_whenCallsUpdate_shouldReturnCategoryUpdated() {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var aCategory = Category.newCategory("Mov", null, expectedIsActive);

    Assertions.assertEquals(0, categoryRepository.count());

    categoryRepository.saveAndFlush(CategoryJpaEntity.from(aCategory));

    Assertions.assertEquals(1, categoryRepository.count());

    final var actualInvalidEntity = categoryRepository.findById(aCategory.getId().getValue()).get();

    Assertions.assertEquals("Mov", actualInvalidEntity.getName());
    Assertions.assertNull(actualInvalidEntity.getDescription());
    Assertions.assertEquals(expectedIsActive, actualInvalidEntity.isActive());

    final var anUpdatedCategory = aCategory.clone()
        .update(expectedName, expectedDescription, expectedIsActive);

    final var actualCategory = categoryGateway.update(anUpdatedCategory);

    Assertions.assertEquals(1, categoryRepository.count());

    Assertions.assertEquals(aCategory.getId(), actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualCategory.getCreatedAt());
    Assertions.assertTrue(aCategory.getUpdatedAt().isBefore(actualCategory.getUpdatedAt()));
    Assertions.assertEquals(aCategory.getDeletedAt(), actualCategory.getDeletedAt());
    Assertions.assertNull(actualCategory.getDeletedAt());

    final var actualEntity = categoryRepository.findById(aCategory.getId().getValue()).get();

    Assertions.assertEquals(aCategory.getId().getValue(), actualEntity.getId());
    Assertions.assertEquals(expectedName, actualEntity.getName());
    Assertions.assertEquals(expectedDescription, actualEntity.getDescription());
    Assertions.assertEquals(expectedIsActive, actualEntity.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualEntity.getCreatedAt());
    Assertions.assertTrue(aCategory.getUpdatedAt().isBefore(actualCategory.getUpdatedAt()));
    Assertions.assertEquals(aCategory.getDeletedAt(), actualEntity.getDeletedAt());
    Assertions.assertNull(actualEntity.getDeletedAt());
  }

  @Test
  public void givenAPrePersistedCategoryAndValidCategoryId_whenTryToDeleteIt_shouldDeleteCategory() {
    final var aCategory = Category.newCategory("Movies", null, true);

    Assertions.assertEquals(0, categoryRepository.count());

    categoryRepository.saveAndFlush(CategoryJpaEntity.from(aCategory));

    Assertions.assertEquals(1, categoryRepository.count());

    categoryGateway.deleteById(aCategory.getId());

    Assertions.assertEquals(0, categoryRepository.count());
  }

  @Test
  public void givenInvalidCategoryId_whenTryToDeleteIt_shouldDeleteCategory() {
    Assertions.assertEquals(0, categoryRepository.count());

    categoryGateway.deleteById(CategoryID.from("invalid"));

    Assertions.assertEquals(0, categoryRepository.count());
  }

  @Test
  public void givenAPrePersistedCategoryAndValidCategoryId_whenCallsFindById_shouldReturnCategory() {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var aCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    Assertions.assertEquals(0, categoryRepository.count());

    categoryRepository.saveAndFlush(CategoryJpaEntity.from(aCategory));

    Assertions.assertEquals(1, categoryRepository.count());

    final var actualCategory = categoryGateway.findById(aCategory.getId()).get();

    Assertions.assertEquals(1, categoryRepository.count());

    Assertions.assertEquals(aCategory.getId(), actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualCategory.getCreatedAt());
    Assertions.assertEquals(aCategory.getUpdatedAt(), actualCategory.getUpdatedAt());
    Assertions.assertEquals(aCategory.getDeletedAt(), actualCategory.getDeletedAt());
    Assertions.assertNull(actualCategory.getDeletedAt());
  }

  @Test
  public void givenValidCategoryIdNotStored_whenCallsFindById_shouldReturnEmpty() {
    Assertions.assertEquals(0, categoryRepository.count());

    final var actualCategory = categoryGateway.findById(CategoryID.from("empty"));

    Assertions.assertTrue(actualCategory.isEmpty());
  }
}
