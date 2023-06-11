package com.pauloreis.catalog.admin.application.category.retrieve.list;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.springframework.beans.factory.annotation.Autowired;

import com.pauloreis.catalog.admin.IntegrationTest;
import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategorySearchQuery;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryJpaEntity;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

import java.util.stream.Stream;

@IntegrationTest
public class ListCategoriesUseCaseIT {

  @Autowired
  private ListCategoriesUseCase useCase;

  @Autowired
  private CategoryRepository categoryRepository;

  @BeforeEach
  void mockUp() {
    final var categories = Stream.of(
        Category.newCategory("Movies", null, true),
        Category.newCategory("Netflix Originals", "Netflix authored titles", true),
        Category.newCategory("Amazon Originals", "Amazon Prime authored titles", true),
        Category.newCategory("Docs", null, true),
        Category.newCategory("Sports", null, true),
        Category.newCategory("Kids", "Category for children", true),
        Category.newCategory("Series", null, true))
        .map(CategoryJpaEntity::from)
        .toList();

    categoryRepository.saveAllAndFlush(categories);
  }

  @Test
  public void givenAValidTerm_whenTermDoesntMatchsPrePersisted_shouldReturnEmptyPage() {
    final var expectedPage = 0;
    final var expectedPerPage = 10;
    final var expectedTerms = "aaa";
    final var expectedSort = "name";
    final var expectedDirection = "asc";
    final var expectedItemsCount = 0;
    final var expectedTotal = 0;

    final var aQuery = new CategorySearchQuery(expectedPage, expectedPerPage, expectedTerms, expectedSort,
        expectedDirection);

    final var actualResult = useCase.execute(aQuery);

    Assertions.assertEquals(expectedItemsCount, actualResult.items().size());
    Assertions.assertEquals(expectedPage, actualResult.currentPage());
    Assertions.assertEquals(expectedPerPage, actualResult.perPage());
    Assertions.assertEquals(expectedTotal, actualResult.total());
  }

  @ParameterizedTest
  @CsvSource({
      "mov,0,10,1,1,Movies",
      "net,0,10,1,1,Netflix Originals",
      "ZON,0,10,1,1,Amazon Originals",
      "KI,0,10,1,1,Kids",
      "children,0,10,1,1,Kids",
      "Amazon,0,10,1,1,Amazon Originals",
  })
  public void givenAValidTerm_whenCallsListCategories_shouldReturnCategoriesFiltered(
      final String expectedTerms,
      final int expectedPage,
      final int expectedPerPage,
      final int expectedItemsCount,
      final long expectedTotal,
      final String expectedCategoryName) {
    final var expectedSort = "name";
    final var expectedDirection = "asc";

    final var aQuery = new CategorySearchQuery(expectedPage, expectedPerPage, expectedTerms, expectedSort,
        expectedDirection);

    final var actualResult = useCase.execute(aQuery);

    Assertions.assertEquals(expectedItemsCount, actualResult.items().size());
    Assertions.assertEquals(expectedPage, actualResult.currentPage());
    Assertions.assertEquals(expectedPerPage, actualResult.perPage());
    Assertions.assertEquals(expectedTotal, actualResult.total());
    Assertions.assertEquals(expectedCategoryName, actualResult.items().get(0).name());
  }

  @ParameterizedTest
  @CsvSource({
      "name,asc,0,10,7,7,Amazon Originals",
      "name,desc,0,10,7,7,Sports",
      "createdAt,asc,0,10,7,7,Movies",
      "createdAt,desc,0,10,7,7,Series",
  })
  public void givenAValidSortAndDirection_whenCallsListCategories_thenShouldReturnCategoriesOrdered(
      final String expectedSort,
      final String expectedDirection,
      final int expectedPage,
      final int expectedPerPage,
      final int expectedItemsCount,
      final long expectedTotal,
      final String expectedCategoryName) {
    final var expectedTerms = "";

    final var aQuery = new CategorySearchQuery(expectedPage, expectedPerPage, expectedTerms, expectedSort,
        expectedDirection);

    final var actualResult = useCase.execute(aQuery);

    Assertions.assertEquals(expectedItemsCount, actualResult.items().size());
    Assertions.assertEquals(expectedPage, actualResult.currentPage());
    Assertions.assertEquals(expectedPerPage, actualResult.perPage());
    Assertions.assertEquals(expectedTotal, actualResult.total());
    Assertions.assertEquals(expectedCategoryName, actualResult.items().get(0).name());
  }

  @ParameterizedTest
  @CsvSource({
      "0,2,2,7,Amazon Originals;Docs",
      "1,2,2,7,Kids;Movies",
      "2,2,2,7,Netflix Originals;Series",
      "3,2,1,7,Sports",
  })
  public void givenAValidPage_whenCallsListCategories_shouldReturnCategoriesPaginated(
      final int expectedPage,
      final int expectedPerPage,
      final int expectedItemsCount,
      final long expectedTotal,
      final String expectedCategoriesName) {
    final var expectedSort = "name";
    final var expectedDirection = "asc";
    final var expectedTerms = "";

    final var aQuery = new CategorySearchQuery(expectedPage, expectedPerPage, expectedTerms, expectedSort,
        expectedDirection);

    final var actualResult = useCase.execute(aQuery);

    Assertions.assertEquals(expectedItemsCount, actualResult.items().size());
    Assertions.assertEquals(expectedPage, actualResult.currentPage());
    Assertions.assertEquals(expectedPerPage, actualResult.perPage());
    Assertions.assertEquals(expectedTotal, actualResult.total());

    int index = 0;
    for (final String expectedName : expectedCategoriesName.split(";")) {
      final String actualName = actualResult.items().get(index).name();
      Assertions.assertEquals(expectedName, actualName);
      index++;
    }
  }
}